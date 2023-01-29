public interface IActivation
{
	bool IsActive { get; }

	public void Activate();
	public void Deactivate();
}