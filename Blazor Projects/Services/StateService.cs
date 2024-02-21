namespace Blazor_Projects.Services;

public class StateService
{
    public bool IsDevelopmentActive;

    public void ChangeDevelopmentMode()
    {
       IsDevelopmentActive = !IsDevelopmentActive;
    }
}
