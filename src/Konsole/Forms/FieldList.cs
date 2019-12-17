namespace Konsole.Forms
{
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

