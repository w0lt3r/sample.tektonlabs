
<!-- PROJECT LOGO -->
</br>
  <h3 align="center">TektonLabs assessment</h3>

  <p align="center">
    ...
    <br />
  </p>
</div>
## Communication Diagram
Markup : ![picture alt](https://github.com/w0lt3r/sample.tektonlabs/communication-diagram.png "Title is optional")
## Project Layers

The project contains three layers. Here

* Web Api: It includes endpoints, middlewares, exceptionhandler, service dependency injection, etc.  
* Core
* Infrastructure


## Data sources

* LazyCache
* Entity Framework Core In Memory
* External Provider (HttpClient)

## Patterns

* Mediator: It receives requests from the Web Aoi layer and handles operations in the core layer.
* Repository Pattern: It abstracts the data access layer.
* Unit of Work: It coordinates the multiple repositories operations.

### Validation technique

* Data Annotation
* Fluent Validation



