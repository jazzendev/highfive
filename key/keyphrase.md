# Keyphrase

Generate private key
```
openssl genpkey -algorithm RSA -aes256 -out private.pem
// PKS8
openssl genpkey -out rsakey.pem -algorithm RSA -pkeyopt rsa_keygen_bits:2048
```

Generate public key
```
openssl rsa -in private.pem -pubout -outform PEM -out public.pem
```

Password
```
testpwd
```

## References

[How To: Generate OpenSSL RSA Key Pair][1]

[Reference 2][2]

[OpenSSL: genrsa vs genpkey?][3]

[Generate PKCS#8 private key with openssl][4]


[1]:https://piechowski.io/post/how-to-use-openssl-genpkey-rsa/
[2]:https://rietta.com/blog/openssl-generating-rsa-key-from-command/
[3]:https://serverfault.com/questions/590140/openssl-genrsa-vs-genpkey
[4]:https://stackoverflow.com/questions/51055884/generate-pkcs8-private-key-with-openssl