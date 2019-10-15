# Completed
● Allow registered users to log in and talk with other users in a chatroom.

● Allow users to post messages as commands into the chatroom with the following format
/stock=stock_code

● Create a decoupled bot that will call an API using the stock_code as a parameter
( https://stooq.com/q/l/?s=aapl.us&f=sd2t2ohlcv&h&e=csv , here aapl.us is the
stock_code )

● The bot should parse the received CSV file and then it should send a message back into
the chatroom using a message broker like RabbitMQ. The message will be a stock quote
using the following format: “APPL.US quote is $93.42 per share”. The post owner will be
the bot.

# Steps to setting 
Create a Database call "challenge", then run the scripts in order structure.sql later data.sql

in the solution replace the Web.config
  <connectionStrings>
    <add name="conn" connectionString="server="name of yourserver"; database=challenge ; integrated security = true" />
  </connectionStrings>
  
  and run


See Use Sample.pdf about how it works!
