using System.Text.Json;

namespace ViaEventAssociation.Infrastructure.EfcQueries.SeedFactories;

public static class GuestSeedFactory
{
    public static List<DTOs.Guest> CreateGuests()
    {
        //absolute path:
        // var json = File.ReadAllText("C:\\Users\\benja\\Desktop\\DCA\\DCA1-VIA_Event_Association\\src\\ViaEventAssociation.Infrastructure.EfcQueries\\SeedFactories\\Guest\\Guests.json");
        // var guests = JsonSerializer.Deserialize<List<DTOs.Guest>>(json);
        
        //const:
        var guests = JsonSerializer.Deserialize<List<DTOs.Guest>>(GuestsJson);
        
        return guests!;
    }

    private const string GuestsJson = """
                                      [
                                        {
                                      	"Id": "6d0e5ef9-294e-4cfb-9575-54427640b6a4",
                                      	"FirstName": "John",
                                      	"LastName": "Doe",
                                      	"Email": "286848@via.dk",
                                      	"PictureUrl": "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcScgIcP58MKd4CpuMNwdTncyXrDwBGmCYXWHA\u0026usqp=CAU"
                                        },
                                        {
                                      	"Id": "9a61ec3a-348d-473d-acb4-3d9decb0eb55",
                                      	"FirstName": "Abdel",
                                      	"LastName": "Abbott",
                                      	"Email": "325031@via.dk",
                                      	"PictureUrl": "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQnPBYGnizEPaBF0QwOfW8VwaKLhW7l_BNZ-g\u0026usqp=CAU"
                                        },
                                        {
                                      	"Id": "d6ef00cc-7862-4660-a78d-e966273cacb6",
                                      	"FirstName": "Abdiel",
                                      	"LastName": "Abel",
                                      	"Email": "282837@via.dk",
                                      	"PictureUrl": "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcScgIcP58MKd4CpuMNwdTncyXrDwBGmCYXWHA\u0026usqp=CAU"
                                        },
                                        {
                                      	"Id": "4bf6523a-0667-41c4-acc9-d09a4e9af430",
                                      	"FirstName": "Abdul",
                                      	"LastName": "Acevedo",
                                      	"Email": "338814@via.dk",
                                      	"PictureUrl": "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcScgIcP58MKd4CpuMNwdTncyXrDwBGmCYXWHA\u0026usqp=CAU"
                                        }
                                      ]
                                      """;
}