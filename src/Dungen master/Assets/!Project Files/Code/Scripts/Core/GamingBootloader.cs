using Core.Project.Base;
using Services;

/// <summary>
/// Этот класс отвечает за начальную загрузку игры.
/// </summary>
public class GamingBootloader : IGamingInitializable
{
    private readonly IProjectEngine _stateMachineService;

    public GamingBootloader(
        IProjectEngine stateMachineService
    )
    {
        _stateMachineService = stateMachineService;
    }

    public void Initialize()
    {
        _stateMachineService.ChangeState<BootstrapState>();
    }
}

public interface IGamingInitializable
{
    void Initialize();
}