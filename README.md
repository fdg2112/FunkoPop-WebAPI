# FunkoPop-WebAPI

## Database Connection

The application uses Entity Framework and expects a connection string named `FunkoPopContext`.
By default, `Data/App.config` contains a placeholder connection string:

```
Data Source=(local);Initial Catalog=funkopop;Integrated Security=True
```

You can override this value by providing a connection string in `Web.config` under
`<connectionStrings>` or by setting the `FUNKOPOP_CONNECTION_STRING` environment
variable. When present, the environment variable takes precedence.
