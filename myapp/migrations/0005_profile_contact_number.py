

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('myapp', '0004_profile'),
    ]

    operations = [
        migrations.AddField(
            model_name='profile',
            name='contact_number',
            field=models.CharField(max_length=15, null=True),
        ),
    ]
