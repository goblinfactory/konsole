using System;
using System.Linq;
using System.Text;
using Konsole.Forms;

namespace Konsole.Forms
{
    public class FieldReader <T>
    {
        private readonly T _object;
        private readonly Type _type;

        public FieldReader(T @object)
        {
            _object = @object;
            _type = typeof(T);
        }

        public FieldList ReadFields()
        {
            var fields = readFields<string>();
            int width = fields.Max(f => f.Caption.Length);
            var fieldlist = new FieldList(fields,width);
            return fieldlist;
        }

        private Field[] readFields<T>()
        {
            var fields = _type
                .GetProperties()
                .Where(f => f.PropertyType == typeof(T))
                .Select(f => new Field(FieldType.String, f.Name, ToCaption(f.Name), (T)f.GetValue(_object)))
                .ToArray();
            return fields;
        }

        private string ToCaption(string caption)
        {
            var sb = new StringBuilder();
            char prev = 'A';
            foreach (var c in caption)
            {
                // if previous was lower and next is upper, add space and next, otherwise add char
                if ((char.IsLower(prev) && char.IsUpper(c)) || (char.IsUpper(prev) && char.IsUpper(c)))
                {
                    sb.Append(' '); 
                }
                sb.Append(c);
                prev = c;
            }
            return sb.ToString();
        }
    }

    public enum FieldType
    {
        String,
        Unsupported
    }

    public class Field
    {
        public FieldType FieldType { get; private set; }
        public string Name { get; private set; }
        public object Value { get; private set; }
        public string Caption { get; private set; }
        public Field(FieldType type, string name, string caption, object value)
        {
            Name = name;
            Caption = caption;
            Value = value;
            FieldType = type;
        }
    }

    public class Field<T> : Field
    {
        public Field(FieldType type, string name, string caption, T value) : base(typeof(T).ParseFieldType(),name,caption, value) {}
    }

    public class StringField : Field<string>
    {
        public StringField(string name, string caption, string value) : base(FieldType.String, name, caption, value) {}
    }


    public class FieldList
    {
        public Field[] Fields { get; private set; }
        public int CaptionWidth { get; private set; }

        public FieldList(Field[] fields, int captionWidth)
        {
            Fields = fields;
            CaptionWidth = captionWidth;
        }
    }
}

internal static class FieldTypeExtensions
{
    public static FieldType ParseFieldType(this Type type)
    {
        if (type == typeof(string)) return FieldType.String;
        return FieldType.Unsupported;
    }
}