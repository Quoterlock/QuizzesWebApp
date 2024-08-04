interface Props{
    results: QuizResult[]
}

export default function QuizResultsList({results}:Props) {
    return (<div>
        { results.map((result, index) =>
            <div key={index} className="block-style mb-3">
                <p>{result.username}</p>
                <p>{result.result}</p>      
            </div>
        )}
    </div>)
}