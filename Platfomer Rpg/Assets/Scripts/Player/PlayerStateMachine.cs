public class PlayerStateMachine//state machine used by player to change state it remembers only current state
{
    public PlayerState currentState { get; private set; }
    public void Initialize(PlayerState _startState)//it is the initialstate that needed to be set in player 

    {
        currentState = _startState;
        currentState.Enter();
    }
    public void ChangeState(PlayerState _newState)//function to change to another state by passing state as parameter
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
}
