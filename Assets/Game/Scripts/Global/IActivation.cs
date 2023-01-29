public interface IActivation
{
	bool IsActive { get; }

	public void Activate();
	public void Deactivate();
}

public interface IActivation<T>
{
	bool IsActive { get; }

	public void Activate(T param);
	public void Deactivate();
}