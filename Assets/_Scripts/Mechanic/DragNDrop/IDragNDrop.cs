using Unity.VisualScripting;

namespace _Scripts.Mechanic.DragNDrop
{
    public interface IDragNDrop
    {
        public DragNDropState State { get; set; }
    }
    public enum DragNDropState{
        None,
        Drag,
        Drop,
    }
}