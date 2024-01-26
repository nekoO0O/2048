public class InputManager
{
    public static InputManager Instance;

    public GameInputControls inputControls;

    public void Init()
    {
        Instance = this;
        inputControls = new GameInputControls();
    }

    public void Enable()
    {
        inputControls.Enable();
    }

    public void Disable()
    {
        inputControls.Disable();
    }
}