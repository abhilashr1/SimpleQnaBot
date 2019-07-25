// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;


namespace Microsoft.BotBuilderSamples.Bots
{
    public class SimpleQnaBot : ActivityHandler
    {
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("qa.json")
                        .Build();

            string question = turnContext.Activity.Text.ToLower().Trim();
            string answer = config.GetValue<string>(question);

            if (!string.IsNullOrEmpty(answer))
                await turnContext.SendActivityAsync(MessageFactory.Text($"{answer}"), cancellationToken);
            else
                await turnContext.SendActivityAsync(MessageFactory.Text($"Sorry, I could not find an answer to that question."), cancellationToken);
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text($"Welcome to the bot! Please type your question. "), cancellationToken);
                }
            }
        }
    }
}
