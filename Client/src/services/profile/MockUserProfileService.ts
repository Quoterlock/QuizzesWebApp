export default class MockUserProfileService implements IUserProfileService {
    async GetUserProfile(id: string) : Promise<UserProfile> {
        return GetMockUser();
    }
    async GetCurrentUserProfile(): Promise<UserProfile> {
        return GetMockUser();
    }
    async UpdateProfile(id: string, profile: UserProfile): Promise<RequesResult> {
        return {message:"not implemented", code:404}
    }
}

function GetMockUser():UserProfile {
    return {
        Username:"mock_user_name",
        Id:"1234",
        DisplayName:"Mock User",
        CompletedQuizzesCount:10,
        CreatedQuizzesCount:10
    }
}