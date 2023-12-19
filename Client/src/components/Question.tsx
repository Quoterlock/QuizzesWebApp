interface Props {
    item: QuizItem
}

export default function Question({item}:Props) {
    return(
        <div>
            <h4>{item.question}</h4>
            {item.answers.map(answer => <p>{answer}</p>)}
        </div>
    )
}