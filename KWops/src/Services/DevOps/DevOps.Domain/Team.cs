using DevOps.Domain;
using Domain;
using System;
using System.Collections.Generic;

public class Team : Entity
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    private readonly List<Developer> _developers;
    public IReadOnlyList<Developer> Developers => _developers;
    private Team(Guid id, string name)
    {
        _developers = new List<Developer>();
        Id = id;
        Name = name;
    }
    public static Team CreateNew(string name)
    {
        Contracts.Require(!string.IsNullOrEmpty(name), "The name of a team cannot be empty");

        return new Team(Guid.NewGuid(), name);
    }
    public void Join(Developer developer)
    {
        if (_developers.Contains(developer))
        {
            throw new ContractException();
        }
        developer.TeamId = Id;
        _developers.Add(developer);
    }
    protected override IEnumerable<object> GetIdComponents()
    {
        yield return Id;
    }
}