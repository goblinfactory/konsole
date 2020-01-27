namespace Konsole
{
    public interface IListView
    {
        (string name, int width)[] Columns { get; set; }
        int selectedItemIndex { get; set; }

        (string name, int width)[] GetResizedColumns();
        void Refresh();
    }
}