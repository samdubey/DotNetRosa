namespace DotNetRosa.core.api
{
    /****/
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /**
     * A state represents a particular state of the application. Each state has an
     * associated view and controller and a set of transitions to new states.
     * 
     * @see TrivialTransitions
     * @author ctsims
     **/
    public interface State
    {
        void start();
    }
}
