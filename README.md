# Stock-Trading-Platform 

Stock-Trading-Platform 

  

Stock trading platform built for the Blackfinch technical test. Requirements available here: Link 

  

  

# Key assumptions made 

  

Various assumptions were made throughout, however the key ones are: 

+ no authentication on any endpoint. 

+ Order queue actioned on first come first served. (i.e. oldest first). 

+ Cannot modify an order once an order is placed. 

+ Sale price does not matter, anywhere in-between min and max is acceptable. 

+ Single product that does not interact with any outside systems. 

  

Assumptions when developing a product for a customer is best avoided at all costs. However due to time constraints(weekend), i was unable to satisfy any assumptions made as with the customer. 

  

# Thoughts and process 

  

While i was given the opportunity to build this technical test in a language i currently use, i decided it would be best to use .NET as the as it would not only showcase my ability to adapt in unfamiliar context but also it would provide me a greater understanding of dot net, its standards and architecture. 

  
Having not built a dot net web api before, first actions were to research and understand the standards and common architecture used. From here clean architecture was chosen as a suitable architecture for this product. 

  
Much like other languages its recommended to split out user interacting functions, core logic and data access. I did so via three projects: Api, Core and Data. 

  

# End Product 

  

+ Constructed in c# with .NET core API.  

  + Singleton is used for storage needs 
  + Use of Dto's to provide value protection

+ Swagger is used to code generate the API specification. 

+ NUnit is used for basic unit tests with minimal cover. 

  

# Improvements 

  

One of the key changes i would made if time permitted and scalability was required, is the migration to microservice architecture. Either through the use of third-party message queue systems (e.g. RabbitMq) or internally through event-based messages (e.g. MediatR) 
