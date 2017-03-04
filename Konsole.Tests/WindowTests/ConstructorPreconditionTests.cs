namespace Konsole.Tests.WindowTests
{
    public class ConstructorPreconditionTests
    {
        // At the moment (since we can't write directly to the native [real] screen buffers, due to platform differences) this is not supported
        // also, the primary use of echo, was only for testing and not for a movable draggable window.
        public void prevent_echoing_to_buffer_that_does_not_full_overlap_child()
        {
            
        }
    }
}
