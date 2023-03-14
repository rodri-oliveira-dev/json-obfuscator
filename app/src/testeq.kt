import java.lang.annotation.Documented
import javax.validation.Constraint
import javax.validation.Payload
import kotlin.reflect.KClass

@Target(AnnotationTarget.FIELD, AnnotationTarget.ANNOTATION_CLASS)
@Retention(AnnotationRetention.RUNTIME)
@Constraint(validatedBy = [MeuValidator::class])
@Documented
annotation class MeuConstraint(
    val message: String = "Validação personalizada falhou",
    val groups: Array<KClass<*>> = [],
    val payload: Array<KClass<out Payload>> = []
)




import javax.validation.ConstraintValidator
import javax.validation.ConstraintValidatorContext

class MeuValidator : ConstraintValidator<MeuConstraint, MeuDTO> {

    override fun isValid(value: MeuDTO?, context: ConstraintValidatorContext): Boolean {
        if (value == null) {
            return false
        }

        // Verifique se o campo1 atende à condição necessária
        val campo1Valido = value.campo1.isNotEmpty()

        // Verifique se o campo2 atende à condição necessária
        val campo2Valido = value.campo2 > 0

        // Retorna verdadeiro se ambos os campos atenderem às condições necessárias
        return campo1Valido && campo2Valido
    }
}






