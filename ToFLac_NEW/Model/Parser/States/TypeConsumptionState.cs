namespace ToFLac_NEW.Model.Parser.States
{
    public class TypeConsumptionState : IState
    {
        public void Enter(StateMachine stateMachine)
        {
            var token = stateMachine.GetCurrentToken();

        }
    }
}
