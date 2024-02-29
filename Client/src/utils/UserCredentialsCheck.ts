function IsEmailValid(email:string) : boolean {
    const emailRegex: RegExp = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
    return emailRegex.test(email)
}

function IsPasswordValid(password:string) : boolean {
    const passwordRegex: RegExp = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,}$/
    return passwordRegex.test(password)
}

export {IsEmailValid, IsPasswordValid}