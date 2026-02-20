using UnityEngine;
namespace MagmaLabs.Editor{
    public class ShowIfAttribute : PropertyAttribute
    {
        public string conditionField;
        public object compareValue;

        public ShowIfAttribute(string conditionField, object value)
        {
            this.conditionField = conditionField;
            this.compareValue = value;
        }
    }

    public class ShowIfNotAttribute : ShowIfAttribute
    {
        public ShowIfNotAttribute(string conditionField, object value) : base(conditionField, value) { }
    }

    public class ShowIfGreaterThanAttribute : ShowIfAttribute
    {
        public ShowIfGreaterThanAttribute(string conditionField, object value) : base(conditionField, value) { }
    }

    public class ShowIfLessThanAttribute : ShowIfAttribute
    {
        public ShowIfLessThanAttribute(string conditionField, object value) : base(conditionField, value) { }
    }

    public class ShowIfGreaterThanOrEqualAttribute : ShowIfAttribute
    {
        public ShowIfGreaterThanOrEqualAttribute(string conditionField, object value) : base(conditionField, value) { }
    }


    public class ShowIfLessThanOrEqualAttribute : ShowIfAttribute
    {
        public ShowIfLessThanOrEqualAttribute(string conditionField, object value) : base(conditionField, value) { }
    }

    
}


