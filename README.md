[![Build Status](https://travis-ci.org/SylvesterMachielse/PrometheusFileServiceDiscoveryApi.svg?branch=master)](https://travis-ci.org/SylvesterMachielse/PrometheusFileServiceDiscoveryApi)

# PrometheusFileServiceDiscoveryApi
An HTTP service for updating a .json file when using Prometheus with 
[file_sd_config](https://prometheus.io/docs/prometheus/latest/configuration/configuration/#%3Cfile_sd_config%3E). 

## Configuration
Add file service discovery to prometheus. If your targets.json file is next to the prometheus.exe, you can add this to the `prometheus.yml` file.
```YAML
 - job_name: 'targets-json'
    file_sd_configs:
    - files:
      - targets.json
```

Add a file `appsettings.json` next to the executable `prom-target-api.exe` (not provided) file.

```javascript
{
    "Host": "http://localhost:6000",
    "TargetFileLocations": [
        {
            "Name": "Production",
            "TargetsFileLocation": "C:\\Workfolder\\prometheus-2.1.0.windows-amd64\\production_targets.json"
        },
        {
            "Name": "Test",
            "TargetsFileLocation": "C:\\Workfolder\\prometheus-2.1.0.windows-amd64\\test_targets.json"
        }
    ]
}

```

## Concept
This is a service that acts as a RESTful API that works with resources called 'targets'. These are just a copy of the targets as described in the [file_sd_config](https://prometheus.io/docs/prometheus/latest/configuration/configuration/#%3Clabelname%3E) configuration. Be sure to honor the conventions of [host](https://prometheus.io/docs/prometheus/latest/configuration/configuration/#%3Chost%3E), [labelname](https://prometheus.io/docs/prometheus/latest/configuration/configuration/#%3Clabelname%3E) and [labelvalue](https://prometheus.io/docs/prometheus/latest/configuration/configuration/#%3Clabelname%3E). The service does **_not_** validate that your models comply with the Prometheus requirements. 

``` javascript
//this is what a `target resource` 
{
    //this is the actual endpoint that will be scraped
    "targets": [
        "mynewtarget.com", //this is a target
	"someothertarget.com" //this is a target
    ],
    "labels": {
        "web": "WEBSERVER-01",      
        "mylabel": "myvalue"      
    }
}
```

* To make life easier (?) the identification of a target resource works with any of the target names in the targets collection. 
* You _can_ update a target inside the targets collection, but the new targets collection must always contain the name of the target you used to identifiy the target resource with.
* A target must be unique within the entire collection of target resources

### Endpoints
#### GET
`http://localhost:6000/api/v1/targets/production`

`http://localhost:6000/api/v1/targets/production/mytarget.com` 

#### PUT
`http://localhost:6000/api/v1/targets/production`

``` javascript
{
    "targets": [
        "mynewtarget.com"
    ],
    "labels": {
        "web": "WEBSERVER-01",      
        "mylabel": "myvalue"      
    }
}
```

#### PATCH
`http://localhost:6000/api/v1/targets/production/mynewtarget.com`

``` javascript
{
    "targets": [
        "mynewtarget.com"
    ],
    "labels": {
         "web": "WEBSERVER-02",       
         "mylabel": "mynewvalue"       
    }
}
```

#### DELETE
`http://localhost:6000/api/v1/targets/production/mytarget.com`
