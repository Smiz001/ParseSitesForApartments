namespace Core.Proxy
{
  public class ProxyInfo
  {
    public string Address { get; set; }
    public int Port { get; set; }
    public string User { get; set; }
    public string Password { get; set; }

    public override string ToString()
    {
      return $"{Address}:{Port}";
    }
  }
}
