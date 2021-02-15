//using System;

//namespace Konsole
//{
//    /// <summary>
//    /// Not threadsafe because KeyHandler delegates directly to Console for keyboard IO and that cannot easily be made threadsafe in any logical manner. e.g. which thread handles a keypress if two threads want to simulate  Console.ReadKey(true) and capture the press?
//    /// </summary>
//    public class ControlEventHandler : IDisposable
//    {
//        private readonly IControl[] _controls;

//        public ControlEventHandler(params IControl[] controls)
//        {
//            _controls = TabOrderer.SetTabOrdering(controls);
//        }

//        public void Dispose()
//        {
//            throw new NotImplementedException();
//        }

//        public void Run()
//        {
//            Console.CursorVisible = false;
//            bool first = true;

//            // render each control in tab order, starting with first control setting to active, and the rest to inactive
//            foreach(var control in _controls)
//            {
//                if (first)
//                {

//                    control.Refresh(ControlStatus.Active);
//                }
//            }
//        }
//    }
//}
