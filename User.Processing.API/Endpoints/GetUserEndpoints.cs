using User.Processing.Service;
using User.Processing.Service.Interface;

namespace User.Processing.API
{
    public static class GetUserEndpoints
    {
        public static void MapGetUserEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/users/{id:int}", async (int id, IExternalUserService userService) =>
            {
                var user = await userService.GetUserByIdAsync(id);
                return Results.Ok(user);
            });

            app.MapGet("/users", async (IExternalUserService userService) =>
            {
                var users = await userService.GetAllUsersAsync();
                return Results.Ok(users);
            });

            app.MapGet("/users/delay/{delay:int}", async (int delay, IExternalUserService userService) =>
            {
                var users = await userService.GetAllUsersWithDelayAsync(delay);
                return Results.Ok(users);
            });
        }
    }
}
