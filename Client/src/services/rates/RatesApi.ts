var apiPath:string

export class RatesApi implements IRatesApi {

    constructor(apiRoute:string) {
        apiPath = `${apiRoute}/rates`
    }

    async RateQuiz(quizId: string, rate: number): Promise<RequesResult> {
        const token = localStorage.getItem("jwn-token")
        const result = await fetch(`${apiPath}/add`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Authorization":`Bearer ${token}`
            },
            body: JSON.stringify({quizId, rate})
        })
        if(result.ok){
            return {code:200, message:"all is ok"}
        } else {
            return {code:result.status, message:result.statusText}
        }
    }
}