namespace Konsole.Forms
{
    public class Field
    {
        public FieldType FieldType { get; private set; }
        public bool Nullable { get; private set; }
        public string Name { get; private set; }
        public object Value { get; private set; }
        public string Caption { get; private set; }
        public Field(FieldType type, string name, string caption, bool nullable, object value)
        {
            Name = name;
            Caption = caption;
            Value = value;
            FieldType = type;
            Nullable = nullable;
        }
    }
}

