public class InputManager
{
    public static InputManager Instance { get; } = new InputManager(); 

    public GameInputControls inputControls;

    private InputManager()
    {
        inputControls = new GameInputControls();
    }

    public void Init()
    {
        inputControls.Enable();
    }

    public void Disable()
    {
        inputControls.Disable();
    }
}