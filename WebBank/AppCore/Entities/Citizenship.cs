﻿namespace WebBank.AppCore.Entities;

public class Citizenship
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public List<Client> Clients { get; set; } = [];
}
