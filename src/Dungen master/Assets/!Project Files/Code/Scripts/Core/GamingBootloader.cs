using Core.Project.Base;
using Services;
using VContainer.Unity;

/// <summary>
/// Этот класс отвечает за начальную загрузку игры.
/// </summary>
public class GamingBootloader : IStartable
{
    private readonly IProjectEngine _stateMachineService;

    public GamingBootloader(
        IProjectEngine stateMachineService
    )
    {
        _stateMachineService = stateMachineService;
    }

    public void Start()
    {
        _stateMachineService.ChangeState<BootstrapState>();
    }
}