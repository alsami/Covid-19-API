# Setup dev https certificate on linux

## Setup self-signed cert file config
1. Create a file called `localhost.conf`
2. Paste the following content into the file 
```config
[req]
default_bits       = 2048
default_keyfile    = localhost.key
distinguished_name = req_distinguished_name
req_extensions     = req_ext
x509_extensions    = v3_ca

[req_distinguished_name]
commonName                  = Common Name (e.g. server FQDN or YOUR name)
commonName_default          = localhost
commonName_max              = 64

[req_ext]
subjectAltName = @alt_names

[v3_ca]
subjectAltName = @alt_names
basicConstraints = critical, CA:false
keyUsage = keyCertSign, cRLSign, digitalSignature,keyEncipherment

[alt_names]
DNS.1   = localhost
DNS.2   = 127.0.0.1
```

## Generate the certificate
1. Execute the following commands in the folder where the file `localhost.conf` is located. **Note**: It will ask for a password, you can set the password or you can leave it blank.
    * `openssl req -x509 -nodes -days 365 -newkey rsa:2048 -keyout localhost.key -out localhost.crt -config localhost.conf`
    * `openssl pkcs12 -export -out localhost.pfx -inkey localhost.key -in localhost.crt`
2. Validate CA file and cert (optional)
    * Optional, verify the CA file: `openssl verify -CAfile localhost.crt localhost.crt`. Result here should be `localhost.crt: OK`
    * Optional, verify that the localhost.crt is **not** trusted yet `openssl verify localhost.crt`. Result here should be **not** successful
    
## Trust the certificate
1. Copy `localhost.crt` to `/usr/local/share/ca-certificates`: `sudo cp localhost.crt /usr/local/share/ca-certificates/localhost.crt`
2. Update certs so that they may be copied to `/etc/ssl/certs/` as a `.pem` file: `sudo update-ca-certificates`
3. Check that the `localhost.pem` file is present in `/etc/ssl/certs/`: `ls /etc/ssl/certs/ | grep localhost.pem`
4. Verify that the cert is accepted in the folder that contains the initially created `localhost.crt`: `openssl verify localhost.crt`. This should yield the following output: `localhost.crt: OK`

## Configure Kestrel
1. Copy the `localhost.pfx` file that was previously exported using the `localhost.crt` file to the web project
2. Make sure it is being copied to the output directory
3. Add the following to "appsettings.Development.json"

```json
{
    "Kestrel": {
        "Certificates": {
          "Default": {
            "Path": "localhost.pfx",
            "Password": "<THE-PASSWORD-OR-BLANK-HERE>"
          }
        }
    }
}
``` 

Now you may reboot your computer and the certificate will be accepted by Kestrel. If you have log-level of Debug you will see that there are errors logged as type debug that may be ignored for development!