## Installing Prerequisites

Make sure you have installed:
* .NET Framework 2.1
* PostgreSQL 9.6
* RabbitMQ 3.7.9

## Run project local

1. Go to Katil.WebAPI/secrets
2. Clone appsettings.Template as appsettings.json (in the same location)
3. Open appsettings.json for edit

```javascript
{
    "ConnectionStrings": {
      "DbConnection": 
        "Host={{localhost}}; Port={{5432}};Database={{ProjectName}};Username={{postgres}};Password={{postgres}};Integrated Security=false;"
    },
    
    "MQ": {
      "Cluster": "host={{localhost}};username=guest;password=guest"
    }
 }
```

4. Set appropriate values - according to your configuration 
Note: please do not commit secrets/appsettings.json to git

