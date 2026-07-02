namespace MiniBillMart.Models;

public sealed class Party
{
    public Party(string code, string name, string address, string state)
    {
        Code = code;
        Name = name;
        Address = address;
        State = state;
    }

    public string Code { get; }

    public string Name { get; }

    public string Address { get; }

    public string State { get; }

    public override string ToString()
    {
        return $"{Name} ({Code})";
    }
}
