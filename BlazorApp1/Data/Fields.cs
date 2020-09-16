using System;

namespace BlazorApp1.Data
{
    public class PrimaryKeyField : FieldElement<int>
    {
    }

    public class TextField : FieldElement<string>
    {
        public bool Multiline { get; set; }
    }

    #region Numeric

    public abstract class NumericField<T> : FieldElement<T>
    {
        public virtual string Format { get; protected set; }
    }

    public class IntegerField : NumericField<int>
    {
        public override string Format { get => "0"; protected set { } }
    }

    public class NullableIntegerField : NumericField<int?>
    {
        public override string Format { get => "0"; protected set { } }
    }

    public class LongField : NumericField<long>
    {
        public override string Format { get => "0"; protected set { } }
    }

    public class NullableLongField : NumericField<long?>
    {
        public override string Format { get => "0"; protected set { } }
    }

    public class DoubleField : NumericField<double>
    {
        public DoubleField()
        {
            Format = "n2";
        }
    }

    public class NullableDoubleField : NumericField<double?>
    {
        public NullableDoubleField()
        {
            Format = "n2";
        }
    }

    public class FloatField : NumericField<float>
    {
        public FloatField()
        {
            Format = "n2";
        }
    }

    public class NullableFloatField : NumericField<float?>
    {
        public NullableFloatField()
        {
            Format = "n2";
        }
    }

    public class DecimalField : NumericField<decimal>
    {
        public DecimalField()
        {
            Format = "n2";
        }
    }

    public class NullableDecimalField : NumericField<decimal?>
    {
        public NullableDecimalField()
        {
            Format = "n2";
        }
    }

    #endregion

    #region DateTime

    public class DateField<T> : FieldElement<T>
    {
        public string Format { get; protected set; } = "dd/MM/yyyy";
    }

    public class DateField : DateField<DateTime>
    {

    }

    public class NullableDateField : DateField<DateTime?>
    {

    }

    public abstract class DateTimeField<T> : FieldElement<T>
    {
        public string Format { get; protected set; } = "dd/MM/yyyy HH:mm:ss";
    }

    public class DateTimeField : DateTimeField<DateTime>
    {

    }

    public class NullableDateTimeField : DateTimeField<DateTime?>
    {

    }

    #endregion

    #region Boolean

    public abstract class BooleanField<T> : FieldElement<T>
    {

    }

    public class BooleanField : BooleanField<bool>
    {

    }

    public class NullableBooleanField : BooleanField<bool?>
    {

    }

    #endregion
}
