﻿using PokemonWebApi.Models;

namespace PokemonWebApi.Interfaces
{
	public interface ICountryRepository
	{
		ICollection<Country> GetCountries();
		Country GetCountry(int id);
		Country GetCountryByOwner(int ownerId);
		ICollection<Owner> GetOwnersFromACountry(int countryId);
		bool CountryExist(int id);
		bool CreateCountry(Country country);
		bool UpdateCountry(Country country);
		bool Save();

	}
}
