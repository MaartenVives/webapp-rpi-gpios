using System;
using System.Device.I2c;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class IndexModel : PageModel
{
    public double CurrentTemperature { get; set; }

    public void OnGet()
    {
        // Hier zou je de code moeten toevoegen om de temperatuur uit de TC74-sensor te lezen
        CurrentTemperature = LeesTemperatuurVanSensor();
    }

    private double LeesTemperatuurVanSensor()
    {
        try
        {
            const int TC74_ADDR = 0x48;
            const byte TEMP_REG = 0x00;

            // Open de I2C-bus (busId: 1 voor meeste moderne Raspberry Pi-modellen)
            using (var i2c = I2cDevice.Create(new I2cConnectionSettings(1, TC74_ADDR)))
            {
                // Lees de temperatuur uit de TC74-sensor
                byte[] rawData = new byte[1];
                i2c.WriteByte(TEMP_REG);
                i2c.Read(rawData);

                // Converteer de ruwe gegevens naar de temperatuurwaarde
                double temperature = rawData[0];

                // Print de temperatuur naar de terminal
                Console.WriteLine($"Gemeten temperatuur: {temperature} °C");

                // Geef de gelezen temperatuur terug
                return temperature;
            }
        }
        catch (Exception ex)
        {
            // Behandel eventuele fouten bij het lezen van de temperatuur
            // Log de fout of neem andere acties op basis van je vereisten
            Console.WriteLine($"Fout bij het lezen van temperatuur: {ex.Message}");
            return double.NaN; // Retourneer een foutwaarde
        }
    }
}
