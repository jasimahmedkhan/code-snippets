namespace program_snippets.Delegates;

public class RetryPattern
{
    // Without Retry (Fragile):
    public static async Task<string> CallApi()
    {
        var httpClient = new HttpClient();
        var response = await httpClient.GetAsync("https://api.example.com/data");
        // 💥 If network fails, the whole program crashes!
        return response.Content.ReadAsStringAsync().Result;
    }
    
    // With Retry (Resilient):
    // Network blips are handled gracefully
    public static async Task<string> CallApiAsync()
    {
        int attempts = 0;
        int maxAttempts = 3;
    
        var httpClient = new HttpClient();
        while (attempts < maxAttempts)
        {
            try
            {
                var response = await httpClient.GetAsync("https://api.example.com/data");
                return response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            { 
                if (attempts >= maxAttempts) 
                    throw;
                Thread.Sleep(1000); // Wait before retry
            }

            attempts++;
        }
        return string.Empty;
    }

    //  Action = a method that returns void
    //  We'll execute this method and retry if it fails
    //  Maximum number of times to try
    public static void Retry(Action action, int maxAttempts)
    {
    //  Counter: "How many times have we tried so far?"
        int attempts = 0;

    //  Loop: "Keep trying while we haven't hit the limit"
        while (attempts < maxAttempts)
    //        ^^^^^^^^^^^^^^^^^^^^^^^^
        {
    //      Try to execute the action
            try
            {
    //          THIS IS WHERE THE ACTUAL WORK HAPPENS
    //          Call the method passed in as a parameter
                action();

                Console.WriteLine("Success!");
    //          SUCCESS! Exit the function immediately
    //          We don't need to retry anymore
                return;
            }
    //      FAILURE! The action threw an exception
            catch (Exception ex)
            {
    //          Increment: "We just tried and failed, count it"
                attempts++;

    //          Log what went wrong
                Console.WriteLine($"Attempt {attempts} failed: {ex.Message}");

    //          Check: "Have we tried enough times?"
                if (attempts >= maxAttempts)
                {
    //              Re-throw the exception to the caller
    //              Let them know we tried our best but failed
                    Console.WriteLine("Max attempts reached. Giving up.");
                    throw;
                }

    //          Wait 1 second before trying again
    //          (Don't hammer the server immediately)
                Thread.Sleep(1000);
            }
        }
    }

    public static void RetryWithDelay(Action action, int maxAttempts, int delayMs = 1000)
    {
        int attempts = 0;

        while (attempts < maxAttempts)
        {
            try
            {
                action();
                Console.WriteLine($"Success on attempt # {attempts + 1}");
                return;
            }
            catch (Exception e)
            {
                attempts++;
                Console.WriteLine($"Attempt #{attempts} failed. Waiting {delayMs}ms before retrying...");
                if (attempts >= maxAttempts)
                {
                    Console.WriteLine("Max attempts reached. Operation failed.");
                    throw;
                }

                Console.WriteLine($"⏳ Waiting {delayMs}ms before retry...");
                Thread.Sleep(delayMs);
            }
        }
    }

    //Why exponential backoff?
    //  Gives the failing service more time to recover
    //  Reduces load on the server (don't hammer it)
    //  Standard practice for API calls
    public static void RetryWithExponentialBackoff(Action action, int maxAttempts, int initialDelayMs = 1000)
    {
        int attempts = 0;
        //  Starting delay (doubles each time)
        //  Track current delay (will increase)
        int currentDelay = initialDelayMs;

        while (attempts < maxAttempts)
        {
            try
            {
                action();
                Console.WriteLine($"✅ Success on attempt {attempts + 1}");
                return;
            }
            catch (Exception ex)
            {
                attempts++;
                Console.WriteLine($"❌ Attempt {attempts}/{maxAttempts} failed: {ex.Message}");
            
                if (attempts >= maxAttempts)
                {
                    Console.WriteLine($"⚠️ All {maxAttempts} attempts failed.");
                    throw;
                }
            
                Console.WriteLine($"⏳ Waiting {currentDelay}ms before retry...");
                Thread.Sleep(currentDelay);
            
                currentDelay *= 2;
            }
        }
    }

    public static T RetryFunc<T>(Func<T> func, int maxAttempts, int delayMs = 1000)
    {
        if (func == null)
        {
            throw new ArgumentNullException(nameof(func), "Function cannot be null");
        }
        
        int attempts = 0;

        while (attempts < maxAttempts)
        {
            try
            {
                // Call the function and get the result
                T result = func();
                Console.WriteLine($"✅ Success on attempt {attempts + 1}");
                // Return the result to caller
                return result;

            }
            catch (Exception ex)
            {
                attempts++;
                Console.WriteLine($"❌ Attempt {attempts}/{maxAttempts} failed: {ex.Message}");

                if (attempts >= maxAttempts)
                {
                    Console.WriteLine($"⚠️ All {maxAttempts} attempts failed.");
                    throw;
                }
                Console.WriteLine($"❌ Attempt #{attempts} failed. Waiting {delayMs}ms before retrying...");
                Thread.Sleep(delayMs);
            }
        }
        //  Compiler requires a return/throw here, but we'll never reach it
        throw new Exception("Max attempts reached. Operation failed.");
    } 
        
}

public class RetryPatternTests
{
    public static void TestRetryPattern()
    {
        // Simulate an unreliable operation
        int attemptCounter = 0;

        Action unreliableOperation = () =>
        {
            attemptCounter++;
            Console.WriteLine($"  [OPERATION] Attempt #{attemptCounter}");
    
            if (attemptCounter < 3)
            {
                // Fail the first 2 attempts
                throw new Exception("Service temporarily unavailable");
            }
    
            // Succeed on the 3rd attempt
            Console.WriteLine("  [OPERATION] Success!");
        };
        // Use retry
        Console.WriteLine("=== Starting retry operation ===");
        RetryPattern.Retry(unreliableOperation, maxAttempts: 5);
    }
    
    public static void TestRetryWithExponentialBackoff()
    {
        int counter = 0;
        Action operation = () =>
        {
            counter++;
            Console.WriteLine($"Attempt #{counter}");
            if (counter < 4)
            {
                throw new Exception("Service temporarily unavailable");
            }

            Console.WriteLine("Success!");
        };
        
        RetryPattern.RetryWithExponentialBackoff(operation, 5, 500);

    }
    
    public static void TestRetryFunc()
    {
        Func<string> getApiData = () =>
        {
            var random = new Random();
            if(random.Next(0, 5) != 0) // 20% chance of failure
            {
                throw new Exception("Network failure - Service temporarily unavailable");
            }
            return "Data fetched successfully!";
        };
        string data = RetryPattern.RetryFunc(getApiData, maxAttempts: 10);
        Console.WriteLine($"Received data: {data}");
    }
    
    
    
}

