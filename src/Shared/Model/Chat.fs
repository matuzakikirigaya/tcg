module Shared.Model.Chat


type ChatSentence = { sentence: string }

type SubmissionFromClient =
    { submittingChatSentence: ChatSentence }
