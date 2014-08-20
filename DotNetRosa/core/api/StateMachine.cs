

namespace DotNetRosa.core.api
{
    /**
 * 
 */
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Collections;

    /**
     * @author ctsims
     *
     */
    public class StateMachine
    {
        private static Stack statesToReturnTo = new Stack();

        public static void setStateToReturnTo(State st)
        {
            statesToReturnTo.Push(st);
        }

        public static State getStateToReturnTo()
        {
            try
            {
                return (State)statesToReturnTo.Pop();
            }
            catch (Exception e)
            {
                throw new Exception("Tried to return to a saved state, but no state to return to had been set earlier in the workflow");
            }
        }
    }

}
