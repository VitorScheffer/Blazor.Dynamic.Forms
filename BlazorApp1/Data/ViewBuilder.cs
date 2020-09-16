using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace BlazorApp1.Data
{
    public class ViewBuilder
    {
        public virtual IEnumerable<IFieldElement> Generate(Type modelType)
        {
            var properties = modelType.GetProperties().Where(w => w.CanRead && w.CanWrite).Select(s => new PropertyModel(s, modelType)).ToArray();              
            foreach (var property in properties)
            {
                foreach (var element in GetFields(property))
                {
                    SetupField(element, property);
                    yield return element;
                }
            }
        }

        protected virtual IEnumerable<IFieldElement> GetFields(PropertyModel property)
        {
            var attributes = property.Attributes.Select(s => s.Attribute).OfType<IFieldAttribute>().ToArray();

            if (!attributes.Any())
            {
                foreach (var field in GetDefaultFieldForType(property))
                {
                    yield return field;
                }
            }
            else 
            {
                foreach (var attribute in attributes)
                {
                    foreach (var field in attribute.GetFields())
                    {
                        yield return field;
                    }
                }
            }
        }

        protected virtual IEnumerable<IFieldElement> GetDefaultFieldForType(PropertyModel property)
        {
            var propertyInfo = property.PropertyInfo;
            if (propertyInfo.Name == "Id")
            {
                yield return new PrimaryKeyField();
            }
            else if (propertyInfo.PropertyType == typeof(string))
            {
                yield return new TextField();
            }
            else if (propertyInfo.PropertyType == typeof(int))
            {
                yield return new IntegerField();
            }
            else if (propertyInfo.PropertyType == typeof(int?))
            {
                yield return new NullableIntegerField();
            }
            else if (propertyInfo.PropertyType == typeof(double))
            {
                yield return new DoubleField();
            }
            else if (propertyInfo.PropertyType == typeof(double?))
            {
                yield return new NullableDoubleField();
            }
            else if (propertyInfo.PropertyType == typeof(float))
            {
                yield return new FloatField();
            }
            else if (propertyInfo.PropertyType == typeof(float?))
            {
                yield return new NullableFloatField();
            }
            else if (propertyInfo.PropertyType == typeof(decimal))
            {
                yield return new DecimalField();
            }
            else if (propertyInfo.PropertyType == typeof(decimal?))
            {
                yield return new NullableDecimalField();
            }
            else if (propertyInfo.PropertyType == typeof(DateTime))
            {
                yield return new DateField();
            }
            else if (propertyInfo.PropertyType == typeof(DateTime?))
            {
                yield return new NullableDateField();
            }
            else if (propertyInfo.PropertyType == typeof(bool))
            {
                yield return new BooleanField();
            }
            else if (propertyInfo.PropertyType == typeof(bool?))
            {
                yield return new NullableBooleanField();
            }

            yield break;
        }

        protected virtual void SetupField(IFieldElement field, PropertyModel property)
        {
            field.BindValue(property);

            //TODO
        }
    }

    public class PropertyModel
    {
        public PropertyInfo PropertyInfo { get; protected set; }
        public string Name { get; protected set; }
        public Type PropertyType { get; protected set; }
        public Type ModelType { get; protected set; }
        public List<AttributeModel> Attributes { get; protected set; }

        public PropertyModel(PropertyInfo propertyInfo, Type modelType)
        {
            PropertyInfo = propertyInfo;
            Name = propertyInfo.Name;
            PropertyType = propertyInfo.PropertyType;
            ModelType = modelType;
            Attributes = propertyInfo.GetCustomAttributes().OfType<IDinamicoAttribute>().Select(ss => new AttributeModel { Attribute = ss }).ToList();
        }

        public virtual object GetPropertyValue(object model)
        {
            return PropertyInfo.GetValue(model);
        }
    }

    public class AttributeModel
    {
        public IDinamicoAttribute Attribute { get; set; }
    }

    public interface IDinamicoAttribute 
    {
    }

    public interface IFieldAttribute : IDinamicoAttribute
    {
        IEnumerable<IFieldElement> GetFields();
    }

    public interface IFieldElement
    {
        void BindValue(PropertyModel propertyModel);
        string GetFieldName();
    }

    public abstract class FieldElement<T> : IFieldElement
    {
        public FieldBind<T> ValueBind { get; protected set; }

        public void BindValue(PropertyModel propertyModel)
        {
            ValueBind = new FieldBind<T>(propertyModel);
        }

        public string GetFieldName()
        {
            return ValueBind.GetFieldName();
        }
    }

    public class FieldBind<T>
    {
        protected Expression<Func<T>> FieldExpression { get; set; }
        protected MethodInfo GetMethod { get; set; }
        protected MethodInfo SetMethod { get; set; }
        protected string FieldName { get; set; }

        public FieldBind(PropertyModel propertyModel)
        {
            FieldName = propertyModel.Name;
            GenerateExpression(propertyModel);
            GetMethod = propertyModel.PropertyInfo.GetGetMethod();
            SetMethod = propertyModel.PropertyInfo.GetSetMethod();
        }

        public virtual T GetValue(object model) 
        {
            return (T) (GetMethod.Invoke(model, null) ?? default(T));
        }

        public virtual void SetValue(object model, T value) 
        {
            SetMethod.Invoke(model, new object[] { value });
        }

        public virtual Expression<Func<T>> GetFieldExpression()
        {
            return FieldExpression;
        }

        public virtual string GetFieldName()
        {
            return FieldName;
        }

        protected virtual void GenerateExpression(PropertyModel property)
        {
            var parameter = Expression.Constant(Activator.CreateInstance(property.ModelType), property.ModelType);
            var propertyAcccess = Expression.PropertyOrField(parameter, property.Name);
            if (typeof(T) == property.PropertyType)
            {
                FieldExpression = Expression.Lambda<Func<T>>(propertyAcccess);
            }
            else
            {
                var converted = Expression.Convert(propertyAcccess, typeof(T));
                FieldExpression = Expression.Lambda<Func<T>>(converted);
            }
        }
    }
}
