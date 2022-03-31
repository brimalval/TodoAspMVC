using TodoWeb.Dtos;

namespace TodoWeb.ViewModels
{
    public class ListIndexViewModel
    {
        public ListIndexViewModel(IEnumerable<TodoListViewDto> allLists) {
            PinnedLists = allLists;
        }
        public IEnumerable<TodoListViewDto> PinnedLists { get; set; }
    }
}