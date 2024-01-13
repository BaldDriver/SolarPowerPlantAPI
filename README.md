# SolarPowerPlantAPI
 
Main goal of this task is to develop a REST API for monitoring and managing solar power plant. The API should allow users to:

• Register and authenticate
• Create, read, update and delete solar power plant with following attributes:

- Solar power plant name (optional)
- Installed power (mandatory)
- Date of installation (mandatory)
-Location longitude and latitude (mandatory)

• Obtain actual or forecasted production data from a solar power plant for a specific period of time and at a preferred level of data granularity.
Each solar power plant has actual production and production forecast records with 15 minutes granularity. Based on the user API request, service should return timeseries based on following attributes:
• Timeseries type – real production or forecasted production
• Timeseries granularity – 15 minutes or 1 hour, where 1 hour granularity is equal to sum of four 15 min records within respective hour
• Timeseries timespan – span for which user would like to obtain timeseries
