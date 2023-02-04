using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyProject_BotDiscord.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {

        [Command("ping")]
        public async Task PingAsync()
        {
            await ReplyAsync("Pong !");
        }

        [Command("avatar")]
        public async Task AvatarAsync(ushort size = 512)
        {
            await ReplyAsync(CDN.GetUserAvatarUrl(Context.User.Id, Context.User.AvatarId, size, ImageFormat.Auto));
        }

        [Command("create-project")]
        public async Task CreateProjectAsync(string projectname, params IGuildUser[] users)
        {
            var guild = Context.Guild;
            var user = Context.User;

            var category = await guild.CreateCategoryChannelAsync($"{projectname.ToUpper()}");

            await category.AddPermissionOverwriteAsync(user, new OverwritePermissions(viewChannel: PermValue.Allow, manageChannel: PermValue.Allow));
            await category.AddPermissionOverwriteAsync(guild.EveryoneRole ,new OverwritePermissions(viewChannel: PermValue.Deny));

            var textChannel = await guild.CreateTextChannelAsync("général", options =>
            {
                options.CategoryId = category.Id;
            });

            var voiceChannel = await guild.CreateVoiceChannelAsync("Vocal", options =>
            {
                options.CategoryId = category.Id;
            });

            foreach (var _user in users)
                await category.AddPermissionOverwriteAsync(_user, new OverwritePermissions(viewChannel: PermValue.Allow));

            await ReplyAsync($"Bravo {user}!! Le project {projectname} à été créer.");
        }

        [Command("addUser-project")]
        public async Task AddUserAsync(string _category, params IGuildUser[] users)
        {
            var category = Context.Guild.CategoryChannels.FirstOrDefault(x => x.Name == _category);

            if (category == null)
            {
                await ReplyAsync($"La catégorie `{_category}` n'a pas été trouvée.");
                return;
            }

            foreach (var user in users)
                await category.AddPermissionOverwriteAsync(user, new OverwritePermissions(viewChannel: PermValue.Allow));

            await ReplyAsync("Permissions ajoutées avec succès.");
        }
    }
}
