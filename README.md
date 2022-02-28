
<!-- PROJECT LOGO -->
</br>
  <h1 align="center">TektonLabs assessment</h1>

  <p align="center">
    ...
    <br />
  </p>
</div>

## Communication Diagram

![communication diagram](https://user-images.githubusercontent.com/71280710/155912341-2604e0e1-95da-443f-a863-120cec81746c.png)

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



