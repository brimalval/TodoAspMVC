using TodoWeb.Dtos;

namespace TodoWeb.ViewModels
{
    public class ListIndexViewModel
    {
        public ListIndexViewModel(IEnumerable<TodoListViewDto> allLists) {
            PinnedLists = allLists.Where(tl => tl.ListState == "Pinned");
            ArchivedLists = allLists.Where(tl => tl.ListState == "Archived");
            OtherLists = allLists
                .Where(tl => !PinnedLists.Any(ptl => ptl.Id == tl.Id))
                .Where(tl => !ArchivedLists.Any(atl => atl.Id == tl.Id)); ;
        }
        public IEnumerable<TodoListViewDto> PinnedLists { get; set; }
        public IEnumerable<TodoListViewDto> ArchivedLists { get; set; }
        public IEnumerable<TodoListViewDto> OtherLists { get; set; }
    }
}