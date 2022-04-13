using CleanArchitecture.Core.ProjectAggregate;
using CleanArchitecture.SharedKernel;

namespace CleanArchitecture.Core.ProjectAggregate.Events;

public class ToDoItemCompletedEvent : BaseDomainEvent
{
  public ToDoItem CompletedItem { get; set; }

  public ToDoItemCompletedEvent(ToDoItem completedItem)
  {
    CompletedItem = completedItem;
  }
}
