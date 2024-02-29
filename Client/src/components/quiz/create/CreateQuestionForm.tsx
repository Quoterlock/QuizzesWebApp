import { ChangeEvent, useState } from "react"

interface Props {
    onChange:(question:QuestionItem) => void
    onRemove:()=>void
    question:QuestionItem
}

export default function CreateQuestionForm({onChange, onRemove, question}:Props) {
    const addOption = () => {
        onChange({...question, options:[...question.options, GetDefaultOption()]})
    }

    const onQuestionLabelChange = (e:ChangeEvent<HTMLInputElement>) => {
        onChange({...question, text:e.currentTarget.value})
    }

    const onOptionChange = (index:number, e:ChangeEvent<HTMLInputElement>) => {
        const newOptions = [...question.options];
        newOptions[index] = {...newOptions[index], text:e.currentTarget.value}
        onChange({...question, options:newOptions})
    }

    const onCorrectAnswerChange = (index:number) => {
        if(question.correctAnswerIndex !== index)
            onChange({...question, correctAnswerIndex:index})
    }
    
    const removeOption = (index:number) => {
        var newQuiestion = question
        newQuiestion.options = question.options.filter((elem,curIndex)=>curIndex !== index)
        onChange(newQuiestion)
    }

    return(<div className="block-style mb-3">
        <div className="container-flex">
            <div className="row mb-2">
                <div className="col mt-1">
                    <label>Question</label>        
                </div>
                <div className="col text-end">
                    <button className="btn minor-btn" onClick={onRemove}>Delete</button>
                </div>
            </div>
        </div>
        
        <div className="d-grid form-inputs mb-3">
            <input type="text" className={(CheckInput(question.text)?"wrong-input":"")} onChange={onQuestionLabelChange}/>
        </div>
        <div >
            <label className="mb-2">Options</label>
            { question.options.map((option,index)=> <div key={index} className="edit-option-style form-inputs mb-2">
            <div className={(question.correctAnswerIndex === index?"correct":"default")+"-answer-select me-2"} onClick={()=>onCorrectAnswerChange(index)}></div>
            <input type="text" className={(CheckInput(option.text) ? "wrong-input" : "") + " option-textbox"} value={option.text} 
                onChange={(e:ChangeEvent<HTMLInputElement>) => onOptionChange(index,e)}/>
            <button className="btn minor-btn ms-2" onClick={() => removeOption(index)}>-</button>
        </div>)}
        </div>
        <div className="d-grid">
            <button className="btn active-btn" onClick={addOption}>Add option</button>
        </div>
    </div>)
}

function GetDefaultOption(): OptionItem {
    return({text:""})
}
function CheckInput(value:string):boolean {
    return (value === null || value === "")
}