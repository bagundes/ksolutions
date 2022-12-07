namespace KS.Core.Base;

public interface IInit
{
    public void Dependencies();
    public void Configure();
    public void Start();
    public void Stop();
    public void Break();
}