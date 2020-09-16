using System;
using System.Collections.Generic;

namespace BlazorApp1.Data
{
    public sealed class TextAreaAttribute : Attribute, IFieldAttribute
    {
        public IEnumerable<IFieldElement> GetFields()
        {
            yield return new TextField { Multiline = true };
        }
    }

    public sealed class IgnoreFieldAttribute : Attribute, IFieldAttribute
    {
        public IEnumerable<IFieldElement> GetFields()
        {
            yield break;
        }
    }

    public sealed class DateTimeAttribute : Attribute, IFieldAttribute
    {
        public IEnumerable<IFieldElement> GetFields()
        {
            yield return new DateTimeField();
        }
    }

    public sealed class NullableDateTimeAttribute : Attribute, IFieldAttribute
    {
        public IEnumerable<IFieldElement> GetFields()
        {
            yield return new NullableDateTimeField();
        }
    }
}
